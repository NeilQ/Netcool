import { Component, OnInit } from '@angular/core';
import { SFSchema } from '@delon/form';
import { NzModalRef } from 'ng-zorro-antd/modal';
import { NotificationService, OrganizationService } from "@services";
import { Organization } from "@models";
import { finalize } from "rxjs/operators";

@Component({
  selector: 'sys-organization-edit',
  templateUrl: './edit.component.html',
})
export class SysOrganizationEditComponent implements OnInit {
  title = '组织';
  initParentId: number = null;
  record: any = {};
  entity: any;
  schema: SFSchema = {
    properties: {
      name: {type: 'string', title: '名称', maxLength: 32},
      description: {type: 'string', title: '说明', maxLength: 256},
      parentId: {
        type: 'number', title: '',
        $comment: '隐藏',
        ui: {
          visibleIf: {parentId: [(value: any) => false]}
        }
      },
    },
    required: ['name'],
  };

  submitting = false;

  constructor(
    private modal: NzModalRef,
    private notificationService: NotificationService,
    private orgService: OrganizationService
  ) {
  }

  ngOnInit(): void {
    if (this.record.id > 0)
      this.orgService.get(this.record.id).subscribe(role => this.entity = role);
    else {
      this.entity = new Organization();
      this.entity.parentId = this.initParentId;
    }
  }

  save(value: any): void {
    this.submitting = true;
    if (this.record.id > 0) {
      this.orgService.update(this.record.id, value)
        .pipe(finalize(() => this.submitting = false))
        .subscribe(() => {
          this.modal.close(true);
        });
    } else {
      this.orgService.add(value)
        .pipe(finalize(() => this.submitting = false))
        .subscribe(() => {
          this.modal.close(true);
        });
    }
  }

  close(): void {
    this.modal.destroy();
  }
}
