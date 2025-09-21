import { Component, OnInit, ViewChild } from '@angular/core';
import { NzModalRef } from 'ng-zorro-antd/modal';
import { NzTreeComponent, NzTreeNode, NzTreeNodeOptions } from "ng-zorro-antd/tree";
import { MenuService, NotificationService, RoleService } from "@services";
import { PermissionType } from "@models";
import { finalize } from "rxjs/operators";
import { SHARED_IMPORTS } from '@shared/shared-imports';

@Component({
  selector: 'app-auth-role-set-permissions',
  templateUrl: './set-permissions.component.html',
  imports: [...SHARED_IMPORTS]
})
export class AuthRoleSetPermissionsComponent implements OnInit {
  record: any = {};
  i: any;

  loading = false;
  submitting = false;

  @ViewChild('tree') tree: NzTreeComponent;
  source: NzTreeNodeOptions[] = [];
  private treeNodeMap: Map<number, NzTreeNodeOptions> = new Map();

  constructor(
    private modal: NzModalRef,
    private notificationService: NotificationService,
    private roleService: RoleService,
    private menuService: MenuService) {
  }

  ngOnInit(): void {
    if (this.record.id > 0) {
      this.i = this.record;
    }
    this.loadMenuTreeNode()
  }

  close() {
    this.modal.destroy();
  }

  loadMenuTreeNode() {
    this.loading = true;
    this.menuService.list({ sort: 'level,order asc' })
      .pipe(finalize(() => this.loading = false))
      .subscribe({
        next: (menus) => {
          if (menus == null || menus.length === 0) {
            return Promise.resolve();
          }

          let tempSource = [];
          menus.forEach(menu => {
            let menuPermissionId = 0;
            if (menu.permissions) {
              // 为v19创建选项数组和值数组
              menu.functionPermissionOptions = menu.permissions.filter((permission) => {
                return permission.type == PermissionType.Function;
              }).map((permission => ({
                label: permission.name,
                value: permission.id
              })));
              
              // 初始化为空的值数组，稍后会根据角色权限填充
              menu.functionPermissions = [];
              let menuPermission = menu.permissions.find(permission => {
                return permission.type == PermissionType.Menu;
              })
              if (menuPermission) {
                menuPermissionId = menuPermission.id;
              }
            }
            const tempNode: NzTreeNodeOptions = {
              title: menu.displayName,
              key: menuPermissionId.toString(),
              children: [],
              selectable: false,
              expanded: true,
              data: menu,
              origin: { data: menu }
            };

            if (menu.parentId > 0 && this.treeNodeMap.has(menu.parentId)) {
              const parent = this.treeNodeMap.get(menu.parentId);
              parent.children.push(tempNode);
              tempNode.parentNode = parent;
              tempNode.level = menu.level;
              tempNode.isLeaf = true;
            } else {
              tempSource.push(tempNode);
            }
            this.treeNodeMap.set(menu.id, tempNode);
          });
          this.source = [...tempSource];
          this.loadSelectedNode();
        }
      })
  }

  loadSelectedNode() {
    if (!this.record) {
      return;
    }

    this.roleService.getPermissions(this.record.id)
      .pipe(finalize(() => this.loading = false))
      .subscribe((permissions) => {
        if (permissions == null) permissions = [];
        const authIdsMap = new Set<number>();
        permissions.forEach((per) => {
          authIdsMap.add(per.id);
        });
      
        this.tree.getTreeNodes().forEach((node) => {
          this.selectRecursive(node, authIdsMap);
        });
      });
  }

  save() {
    if (!this.record) {
      return;
    }

    this.submitting = true;
    let tempPermissionIds: number[] = [];

    this.tree.getTreeNodes().forEach(node => {
      fetchPermissionId(node, tempPermissionIds);
    });

    tempPermissionIds = tempPermissionIds.sort();

    function fetchPermissionId(node: NzTreeNode, permissionIds: number[]) {
      if (node.isChecked || node.isHalfChecked) {
        permissionIds.push(Number(node.key));
        if (node.origin.data && node.origin.data.functionPermissions) {
          // functionPermissions 现在是值的数组，直接添加到 permissionIds 中
          node.origin.data.functionPermissions.forEach(permissionId => {
            permissionIds.push(permissionId);
          });
        }
        if (node.children) {
          node.children.forEach(v => {
            fetchPermissionId(v, permissionIds);
          });
        }
      }
    }

    this.submitting = true;
    this.roleService.setPermissions(this.record.id, tempPermissionIds)
      .pipe(finalize(() => {
        this.submitting = false;
      }))
      .subscribe(() => {
        this.notificationService.successMessage("保存成功");
        this.modal.close(true);
      });
  }

  updateAllChecked(menu): void {
    menu.functionIndeterminate = false;
    if (menu.functionAllChecked) {
      // 选中所有功能权限
      menu.functionPermissions = menu.functionPermissionOptions.map(option => option.value);
    } else {
      // 取消选中所有功能权限
      menu.functionPermissions = [];
    }
  }

  updateSingleChecked(menu): void {
    const totalCount = menu.functionPermissionOptions ? menu.functionPermissionOptions.length : 0;
    const selectedCount = menu.functionPermissions ? menu.functionPermissions.length : 0;
    
    if (selectedCount === 0) {
      menu.functionAllChecked = false;
      menu.functionIndeterminate = false;
    } else if (selectedCount === totalCount) {
      menu.functionAllChecked = true;
      menu.functionIndeterminate = false;
    } else {
      menu.functionAllChecked = false;
      menu.functionIndeterminate = true;
    }
  }

  nodeCheckChange(e) {
    if (!e.node.isChecked) {
      this.clearFunctionPermissions(e.node.origin.data);
    }
  }


  private clearFunctionPermissions(menu) {
    if (menu.functionPermissions) {
      menu.functionAllChecked = false;
      menu.functionIndeterminate = false;
      menu.functionPermissions = [];
    }
  }

  private selectRecursive(node: NzTreeNode, permissionIds: Set<number>) {
    if (node.origin.data.functionPermissionOptions) {
      // 根据权限ID设置选中的功能权限
      node.origin.data.functionPermissions = node.origin.data.functionPermissionOptions
        .filter(option => permissionIds.has(option.value))
        .map(option => option.value);
      
      // 更新全选状态
      this.updateSingleChecked(node.origin.data);
    }
    if (node.children) {
      node.children.forEach(childNode => {
        this.selectRecursive(childNode, permissionIds);
      });

      if (node.children.length === 0) {
        // 最后子节点
        if (permissionIds.has(Number(node.key))) {
          // 选中
          node.isChecked = true;
          node.update();
        } else {
          // 取消选中
          node.isChecked = false;
          node.isHalfChecked = false;
          node.update();
        }
      } else {
        if (permissionIds.has(Number(node.key))) {
          node.isChecked = true;
          node.update();
        } else {
          node.isChecked = false;
          node.isHalfChecked = false;
          node.update();
        }
      }
    }
  }

}

