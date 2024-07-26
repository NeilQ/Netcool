import { Component, OnInit } from '@angular/core';
import { ControlWidget, DelonFormModule } from '@delon/form';
import E from 'wangeditor';

@Component({
  selector: 'sf-wang-editor',
  template: `
    <sf-item-wrap [id]="id" [schema]="schema" [ui]="ui" [showError]="showError" [error]="error"
                  [showTitle]="schema.title">
      <!-- 开始自定义控件区域 -->

      <div [id]="editorId"></div>

      <!-- 结束自定义控件区域 -->
    </sf-item-wrap>`,
  standalone: true,
  imports: [DelonFormModule]
})
export class WangEditorWidget extends ControlWidget implements OnInit {
  /* 用于注册小部件 KEY 值 */
  static readonly KEY = 'wang-editor';

  // 组件所需要的参数，建议使用 `ngOnInit` 获取
  editorId: string;

  ngOnInit(): void {
    this.editorId = 'wang_editor' + this.id;

    // import { DropListConf, DropListItem, PanelTabConf, PanelConf, TooltipConfItemType } from 'wangeditor'

    //const { $, BtnMenu, DropListMenu, PanelMenu, DropList, Panel, Tooltip } = E
    //console.log($, BtnMenu, DropListMenu, PanelMenu, DropList, Panel, Tooltip)

    setTimeout(() => {
      const editor = new E('#' + this.editorId);

      editor.config.uploadImgServer = '/api/files/upload/wang-editor';
      editor.config.uploadImgMaxLength = 1;

      if (this.ui.config) {
        Object.assign(editor.config, this.ui.config);
      }

      editor.config.onchange = (newHtml: any) => {
        this.change(newHtml);
        //let content: HTMLElement = document.getElementById('content') as HTMLElement
        //content.innerHTML = newHtml
      }
      editor.create();
      editor.txt.html(this.value);
    })
  }

  // reset 可以更好的解决表单重置过程中所需要的新数据问题
  reset(value: string) {

  }

  change(value: string) {
    if (this.ui.change) this.ui.change(value);
    this.setValue(value);
  }
}
