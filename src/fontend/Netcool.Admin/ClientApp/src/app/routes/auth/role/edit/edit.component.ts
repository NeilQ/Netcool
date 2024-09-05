import { Component, OnInit } from '@angular/core';
import { NzModalRef } from 'ng-zorro-antd/modal';
import { SFSchema } from '@delon/form';
import { RoleService } from "@services";
import { NotificationService } from "@services";
import { Role } from "@models";
import { finalize } from "rxjs/operators";

@Component({
  selector: 'auth-role-edit',
  templateUrl: './edit.component.html',
})
export class AuthRoleEditComponent implements OnInit {
  title = '角色';
  record: any = {};
  entity: any;
  schema: SFSchema = {
    properties: {
      name: {type: 'string', title: '名称', maxLength: 32},
      notes: {type: 'string', title: '备注', maxLength: 256},
    },
    required: ['name'],
  };
  submitting = false;

  constructor(
    private modal: NzModalRef,
    private notificationService: NotificationService,
    private roleService: RoleService) {
  }

  ngOnInit(): void {
    if (this.record.id > 0)
      this.roleService.get(this.record.id).subscribe(role => this.entity = role);
    else
      this.entity = new Role();
  }

  save(value: any) {
    this.submitting = true;
    if (this.record.id > 0) {
      this.roleService.update(this.record.id, value)
        .pipe(finalize(() => this.submitting = false))
        .subscribe(() => {
          this.modal.close(true);
        });
    } else {
      this.roleService.add(value)
        .pipe(finalize(() => this.submitting = false))
        .subscribe(() => {
          this.modal.close(true);
        });
    }
  }

  close() {
    this.modal.destroy();
  }
}
