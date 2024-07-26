import { Component } from '@angular/core';
import { ModalHelper } from '@delon/theme';
import { RoleService } from "@services";
import { AuthRoleEditComponent } from "./edit/edit.component";
import { NotificationService } from "@services";
import { CrudTableComponentBase } from "@models";
import { Role } from "@models";
import { AuthRoleSetPermissionsComponent } from "./set-permissions/set-permissions.component";
import { SFSchema } from "@delon/form";
import { STColumn } from "@delon/abc/st";

@Component({
  selector: 'auth-role',
  templateUrl: './role.component.html',
})
export class AuthRoleComponent extends CrudTableComponentBase<Role> {

  editComponent = AuthRoleEditComponent;

  searchSchema: SFSchema = {
    properties: {
      name: {type: 'string', title: '账号名称', maxLength: 32},
    }
  };

  constructor(protected apiService: RoleService,
              protected modal: ModalHelper,
              protected notificationService: NotificationService) {
    super(apiService, modal, notificationService);
    this.deleteConfirmMessage = '删除角色将移除与用户的关联，是否继续？';
    this.columns = [
      {title: 'id', index: 'id', type: 'checkbox'},
      {title: '名称', width: "200px", index: 'name'},
      {title: '备注', index: 'notes'},
      {
        title: '操作', width: "200px",
        buttons: [
          {
            text: '编辑', icon: 'edit', type: 'modal',
            modal: {component: this.editComponent, params: (record) => Object},
            acl: this.permissions.roleUpdate,
            click: () => this.onSaveSuccess()
          },
          {
            text: '设置权限', icon: 'setting', type: 'modal',
            acl: this.permissions.roleSetPermissions,
            modal: {component: AuthRoleSetPermissionsComponent, params: (record) => Object}
          },
        ]
      }
    ] as STColumn[];
  }

}
