import { Component, OnInit, ViewChild } from '@angular/core';
import { NzModalRef } from 'ng-zorro-antd/modal';
import { NzTreeComponent, NzTreeNode, NzTreeNodeOptions } from "ng-zorro-antd/tree";
import { MenuService, NotificationService, RoleService } from "@services";
import { PermissionType } from "@models";
import { finalize } from "rxjs/operators";

@Component({
  selector: 'app-auth-role-set-permissions',
  templateUrl: './set-permissions.component.html',
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
    this.menuService.list({sort: 'level,order asc'})
      .subscribe((menus) => {
        if (menus == null || menus.length === 0) {
          return Promise.resolve();
        }

        let tempSource = [];
        menus.forEach(menu => {
          let menuPermissionId = 0;
          if (menu.permissions) {
            menu.functionPermissions = menu.permissions.filter((permission) => {
              return permission.type == PermissionType.Function;
            }).map((permission => {
              permission.label = permission.name;
              permission.value = permission.id;
              return permission;
            }));
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
            origin: {data: menu}
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
      }, error => {
        this.loading = false;
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
          node.origin.data.functionPermissions.forEach(v => {
            if (v.checked) {
              permissionIds.push(v.id);
            }
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
      menu.functionPermissions.forEach(item => item.checked = true);
    } else {
      menu.functionPermissions.forEach(item => item.checked = false);
    }
  }

  updateSingleChecked(menu): void {
    if (menu.functionPermissions.every(item => item.checked === false)) {
      menu.functionAllChecked = false;
      menu.functionIndeterminate = false;
    } else if (menu.functionPermissions.every(item => item.checked === true)) {
      menu.functionAllChecked = true;
      menu.functionIndeterminate = false;
    } else {
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
      menu.functionPermissions.forEach(v => {
        v.checked = false;
      });
    }
  }

  private selectRecursive(node: NzTreeNode, permissionIds: Set<number>) {
    if (node.origin.data.functionPermissions) {
      let tempCount = 0;
      node.origin.data.functionPermissions.forEach(v => {
        if (permissionIds.has(v.id)) {
          v.checked = true;
          tempCount++;
        } else {
          v.checked = false;
        }
      });
      node.origin.data.functionAllChecked = tempCount === node.origin.data.functionPermissions.length;
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

