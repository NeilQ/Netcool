import type { SFWidgetProvideConfig } from '@delon/form';

import { TestWidget } from './test/test.widget';
import { WangEditorWidget } from "@shared/json-schema/widgets/wang-editor/wang-editor.widget";
import { TransferWidget } from "@delon/form/widgets/transfer";
import { TreeSelectWidget } from "@delon/form/widgets/tree-select";

export const SF_WIDGETS: SFWidgetProvideConfig[] = [
  {KEY: TestWidget.KEY, type: TestWidget},
  {KEY: TransferWidget.KEY, type: TransferWidget},
  {KEY: TreeSelectWidget.KEY, type: TreeSelectWidget},
  {KEY: WangEditorWidget.KEY, type: WangEditorWidget}
];
