import { Component, OnInit } from '@angular/core';
import { NzModalRef } from 'ng-zorro-antd/modal';
import { SFSchema  } from '@delon/form';
import type { SFTreeSelectWidgetSchema } from '@delon/form/widgets/tree-select';
import { EnumService, OrganizationService, UserService } from "@services";
import { NotificationService } from "@services";
import { User } from "@models";
import { finalize } from "rxjs/operators";

@Component({
  selector: 'auth-user-edit',
  templateUrl: './edit.component.html',
})
export class AuthUserEditComponent implements OnInit {
  title = '用户';
  record: any = {};
  entity: any;
  schema: SFSchema = {
    properties: {
      name: {type: 'string', title: '账号名称', maxLength: 32},
      displayName: {type: 'string', title: '昵称', maxLength: 256},
      isActive: {
        type: 'boolean', title: '是否启用',
        ui: {
          widget: 'checkbox',
        }
      },
      gender: {
        type: 'number', title: '性别',
        ui: {
          widget: 'select',
        },
        enum: this.enumService.getEnum('gender').map(t => ({label: t.name, value: t.value}))
      },
      email: {
        type: 'string',
        title: '邮箱',
        maxLength: 256,
        format: 'email'
      },
      phone: {type: 'string', title: '电话', maxLength: 64, format: 'mobile'},
      organizationId: {
        type: 'number', title: '组织',
        ui: {
          widget: 'tree-select',
          asyncData: () => this.orgService.getTreeEnumOptions(),
          defaultExpandAll: true,
        } as SFTreeSelectWidgetSchema,
      }
    },
    required: ['name', 'gender'],
  };

  submitting = false;

  constructor(
    private modal: NzModalRef,
    private notificationService: NotificationService,
    private enumService: EnumService,
    private orgService: OrganizationService,
    private apiService: UserService) {
  }

  ngOnInit(): void {
    if (this.record.id > 0)
      this.apiService.get(this.record.id).subscribe(data => this.entity = data);
    else
      this.entity = new User();
  }

  save(value: any) {
    this.submitting = true;
    if (this.record.id > 0) {
      this.apiService.update(this.record.id, value)
        .pipe(finalize(() => {
          this.submitting = false;
        }))
        .subscribe(() => {
          this.modal.close(true);
        });
    } else {
      this.apiService.add(value)
        .pipe(finalize(() => {
          this.submitting = false;
        }))
        .subscribe(() => {
          this.modal.close(true);
        });
    }
  }

  close() {
    this.modal.destroy();
  }

}
