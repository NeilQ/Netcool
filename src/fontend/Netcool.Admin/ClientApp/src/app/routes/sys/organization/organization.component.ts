import { Component, OnInit, ViewChild } from '@angular/core';
import { STColumn, STComponent } from '@delon/abc/st';
import { SFSchema } from '@delon/form';
import { NzFormatEmitEvent, NzTreeNode, NzTreeNodeOptions } from "ng-zorro-antd/tree";
import { Menu, Organization } from "@models";
import { NotificationService, OrganizationService } from "@services";
import { NzModalService } from "ng-zorro-antd/modal";
import { SysOrganizationEditComponent } from "./edit/edit.component";
import { ModalHelper } from "@delon/theme";
import { finalize } from "rxjs/operators";

@Component({
  selector: 'sys-organization',
  styleUrls: ['./organization.component.scss'],
  templateUrl: './organization.component.html',
})
export class SysOrganizationComponent implements OnInit {

  nodes: NzTreeNodeOptions[] = [];
  nzSelectedKeys = [];
  currentOrganization: Organization;

  isDeleting = false;
  isLoading = false;

  constructor(private organizationService: OrganizationService,
              private modalService: NzModalService,
              protected modal: ModalHelper,
              private  notificationService: NotificationService) {
  }

  ngOnInit(): void {
    this.loadTree();
  }

  onAdd() {
    this.modal
      .createStatic(SysOrganizationEditComponent,
        {initParentId: this.currentOrganization == null ? null : this.currentOrganization.id})
      .subscribe(() => {
        this.onSaveSuccess();
      });
  }

  delete() {
    this.isDeleting = true;
    this.organizationService.delete([this.currentOrganization.id])
      .pipe(finalize(() => this.isDeleting = false))
      .subscribe(() => {
        this.onDeleteSuccess();
      });
  }

  onEdit() {
    if (this.currentOrganization == null) {
      this.notificationService.warningMessage("请选择");
      return;
    }
    this.modal
      .createStatic(SysOrganizationEditComponent,
        {
          record: this.currentOrganization
        })
      .subscribe(() => {
        this.onSaveSuccess();
      });
  }

  onSaveSuccess() {
    this.notificationService.successMessage("保存成功");
    this.loadTree();
  }

  onDeleteSuccess() {
    this.notificationService.successMessage("删除成功");
    this.currentOrganization = null;
    this.loadTree();
  }

  loadTree() {
    this.isLoading = true;
    this.organizationService.list({sort: "depth,id"})
      .pipe(finalize(() => this.isLoading = false))
      .subscribe(data => {
        let root = {
          title: '全部组织',
          key: 'root',
          expanded: true,
          children: [],
          isLeaf: false,
        };
        let map = new Map<number, NzTreeNodeOptions>();
        data.forEach((value) => {
          if (this.currentOrganization != null && this.currentOrganization.id == value.id) {
            this.currentOrganization = value;
          }
          if (value.parentId == null || value.parentId <= 0) {
            let node = {
              title: value.name,
              key: value.id.toString(),
              expanded: true,
              children: [],
              icon: "apartment",
              isLeaf: true,
              data: value
            };
            map.set(value.id, node);
            root.children.push(node);
          } else {
            let node = {
              title: value.name,
              key: value.id.toString(),
              expanded: true,
              children: [],
              isLeaf: true,
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
          if (this.currentOrganization != null) {
            this.nzSelectedKeys = [this.currentOrganization.id.toString()];
            this.showInfo(this.currentOrganization);
          }
        } else {
          this.currentOrganization = null;
        }
      })
  }

  activeNode(data: NzFormatEmitEvent) {
    if (data == null || data.node.key == 'root') {
      this.currentOrganization = null;
      return;
    }
    this.showInfo(data.node!.origin.data)
  }

  showInfo(org: Organization) {
    this.currentOrganization = org;
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
