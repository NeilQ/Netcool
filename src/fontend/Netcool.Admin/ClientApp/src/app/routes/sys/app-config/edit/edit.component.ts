import { Component, OnInit } from '@angular/core';
import { NzModalRef } from 'ng-zorro-antd/modal';
import { SFSchema } from '@delon/form';
import { AppConfigService } from "@services";
import { NotificationService } from "@services";
import { AppConfig } from "@models";
import { finalize } from "rxjs/operators";

@Component({
  selector: 'sys-config-edit',
  templateUrl: './edit.component.html',
})
export class SysAppConfigEditComponent implements OnInit {
  title = '应用设置';
  record: any = {};
  entity: any;
  schema: SFSchema = {
    properties: {
      name: {type: 'string', title: '名称', maxLength: 32},
      value: {type: 'string', title: '值', maxLength: 256},
      description: {type: 'string', title: '说明', maxLength: 256},
    },
    required: ['name', 'value'],
  };
  submitting = false;

  constructor(
    private modal: NzModalRef,
    private notificationService: NotificationService,
    private appConfigService: AppConfigService) {
  }

  ngOnInit(): void {
    if (this.record.id > 0)
      this.appConfigService.get(this.record.id).subscribe(role => this.entity = role);
    else
      this.entity = new AppConfig();
  }

  save(value: any) {
    this.submitting = true;
    if (this.record.id > 0) {
      this.appConfigService.update(this.record.id, value)
        .pipe(finalize(() => this.submitting = false))
        .subscribe(() => {
          this.modal.close(true);
        });
    } else {
      this.appConfigService.add(value)
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
