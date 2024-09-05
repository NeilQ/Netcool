import { Component, OnInit, ViewChild } from '@angular/core';
import { STColumn, STComponent } from '@delon/abc/st';
import { NzFormatEmitEvent, NzTreeNode, NzTreeNodeOptions } from "ng-zorro-antd/tree";
import { MenuService } from "@services";
import { Menu } from "@models";
import { finalize } from "rxjs/operators";

@Component({
  selector: 'auth-menu',
  styleUrls: ['./menu.component.scss'],
  templateUrl: './menu.component.html',
})
export class AuthMenuComponent implements OnInit {

  nodes: NzTreeNodeOptions[] = [];
  nzSelectedKeys = [];
  currentMenu: Menu;

  isLoading = false;

  @ViewChild('st', {static: false}) st: STComponent;
  columns: STColumn[] = [
    {title: '权限名称', index: 'name'},
    {title: '权限代码', index: 'code'},
    {title: '类型', index: 'typeDescription'},
    {title: '描述', index: 'notes'}
  ];

  constructor(private menuService: MenuService) {
  }

  ngOnInit() {
    this.loadTree();
  }

  loadTree() {
    this.isLoading = true;
    this.menuService.list({sort: "level,order"})
      .pipe(finalize(() => this.isLoading = false))
      .subscribe(data => {
        let root = {
          title: '全部菜单',
          key: 'root',
          expanded: true,
          children: [],
          isLeaf: false,
        };
        let map = new Map<number, NzTreeNodeOptions>();
        data.forEach((value) => {
          if (value.parentId == null || value.parentId <= 0) {
            let node = {
              title: value.displayName,
              key: value.id.toString(),
              expanded: true,
              children: [],
              isLeaf: true,
              data: value
            }
            map.set(value.id, node);
            root.children.push(node);
          } else {
            let node = {
              title: value.displayName,
              key: value.id.toString(),
              expanded: true,
              children: [],
              isLeaf: true,
              icon: value.icon,
              data: value
            }
            let parentNode = map.get(value.parentId);
            if (parentNode != null) {
              parentNode.isLeaf = false;
              parentNode.children.push(node);
            }
            map.set(value.id, node);
          }
        });
        this.nodes = [root];
        if (root.children.length > 0) {
          this.nzSelectedKeys = [root.children[0].key];
          this.showInfo(root.children[0].data);
        }
      })
  }

  activeNode(data: NzFormatEmitEvent) {
    if (data == null || data.node.key == 'root') {
      this.currentMenu = null;
      return;
    }
    this.showInfo(data.node!.origin.data)
  }

  showInfo(menu: Menu) {
    this.currentMenu = menu
  }

  expendNode(data: NzTreeNode | NzFormatEmitEvent): void {
    if (data instanceof NzTreeNode) {
      data.isExpanded = !data.isExpanded;
    } else {
      const node = data.node;
      if (node) {
        node.isExpanded = !node.isExpanded;
      }
    }
  }


}
