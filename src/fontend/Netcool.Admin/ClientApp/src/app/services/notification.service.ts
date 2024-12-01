import { Injectable } from '@angular/core';
import { NzMessageService } from "ng-zorro-antd/message";
import { NzModalService } from "ng-zorro-antd/modal";
import { NzNotificationService } from "ng-zorro-antd/notification";

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  constructor(private modalService: NzModalService,
              private message: NzMessageService,
              private notification: NzNotificationService) {
  }

  successMessage(content?: string) {
    this.message.success(content);
  }

  infoMessage(content?: string) {
    this.message.success(content);
  }

  warningMessage(content?: string) {
    this.message.warning(content);
  }

  errorMessage(content?: string) {
    this.message.error(content);
  }

  errorNotification(title: string, content?: string, type?: string) {
    if (type != null && type === 'message') {
      this.message.error(content);
      return;
    }

    this.notification.create('error', title, content);
  }

  infoNotification(title: string, content?: string) {
    this.notification.create('info', title, content);
  }

  warningNotification(title: string, content?: string) {
    this.notification.create('warning', title, content);
  }

  confirmDeleteModal(onOk: Function) {
    this.modalService.confirm({
      nzTitle: '确定要删除该记录吗？',
      nzContent: '',
      nzOkDanger: true,
      nzOnOk: () => onOk(),
    });
  }

  confirmModal(message: string, onOk: Function) {
    this.modalService.confirm({
      nzTitle: message,
      nzContent: '',
      nzOkDanger: true,
      nzOnOk: () => onOk(),
    });
  }
}
