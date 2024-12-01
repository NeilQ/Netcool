import {Component, OnInit} from '@angular/core';
import {NzModalRef} from 'ng-zorro-antd/modal';
import {SFSchema} from '@delon/form';
import {NotificationService, UserService} from "@services";
import {finalize} from "rxjs/operators";

@Component({
  selector: 'app-auth-user-reset-password',
  templateUrl: './reset-password.component.html',
})
export class AuthUserResetPasswordComponent implements OnInit {
  record: any = {};
  model: { new: string, confirm: string } = {new: '', confirm: ''};
  schema: SFSchema = {
    properties: {
      new: {type: 'string', title: '新密码', minLength: 5, maxLength: 32},
      confirm: {type: 'string', title: '确认密码', minLength: 5, maxLength: 32},
    },
    required: ['new', 'confirm'],
  };

  submitting = false;

  constructor(
    private modal: NzModalRef,
    private notificationService: NotificationService,
    private apiService: UserService) {
  }

  ngOnInit(): void {
  }

  save(value: any) {
    if (!this.record) return;
    this.submitting = true;
    this.apiService.resetPassword(this.record.id, value.new, value.confirm)
      .pipe(finalize(() => {
        this.submitting = false;
      }))
      .subscribe(() => {
        this.notificationService.successMessage('保存成功');
        this.modal.close(true);
      });
  }

  close() {
    this.modal.destroy();
  }
}
