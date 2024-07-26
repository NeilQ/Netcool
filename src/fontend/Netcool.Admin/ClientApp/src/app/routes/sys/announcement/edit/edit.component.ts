import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { SFSchema } from '@delon/form';
import { NzModalRef } from 'ng-zorro-antd/modal';
import { AnnouncementService, NotificationService } from "@services";
import { Announcement } from "@models";
import { finalize } from "rxjs/operators";

@Component({
  selector: 'sys-announcement-edit',
  templateUrl: './edit.component.html',
  encapsulation: ViewEncapsulation.None
})
export class SysAnnouncementEditComponent implements OnInit {
  title = '公告';
  record: any = {};
  entity: any;
  schema: SFSchema = {
    properties: {
      title: {type: 'string', title: '标题', maxLength: 32},
      body: {
        type: 'string', title: '内容', ui: {
          widget: "wang-editor",
          config: {height: 500},
          grid: {
            span: 24
          }
        }
      },
    },
    required: ['title', 'value'],
  };
  submitting = false;

  constructor(
    private modal: NzModalRef,
    private notificationService: NotificationService,
    private apiService: AnnouncementService
  ) {
  }

  ngOnInit(): void {
    if (this.record.id > 0)
      this.apiService.get(this.record.id).subscribe(role => this.entity = role);
    else
      this.entity = new Announcement();
  }

  save(value: any) {
    this.submitting = true;
    if (this.record.id > 0) {
      this.apiService.update(this.record.id, value)
        .pipe(finalize(() => this.submitting = false))
        .subscribe(() => {
          this.modal.close(true);
        });
    } else {
      this.apiService.add(value)
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
