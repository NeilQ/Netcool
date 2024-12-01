import { OnDestroy, OnInit, Directive } from "@angular/core";
import { CrudRestServiceBase, NotificationService } from "@services";
import { Observable } from "rxjs";
import { tap } from "rxjs/operators";
import { PagedResult } from "./dto.common";
import { ModalHelper } from "@delon/theme";
import { Permissions } from "./permissions";
import { deepCopy } from "@delon/util";
import { STChange, STColumn, STData, STPage } from "@delon/abc/st";

@Directive()
export abstract class TableComponentBase<TEntity = any> implements OnInit, OnDestroy {

  permissions: Permissions = Permissions.Instance;
  data: STData[] = [];
  loading: boolean = false;
  total: number = 0;
  pageIndex: number = 1;
  pageSize: number = 10;

  /*
   * 选中的行数据项
   */
  selectedItems: STData[] = [];

  /*
   * 查询条件对象
   */
  query: any;

  /*
   * 分页组件配置
   */
  page: STPage = {
    front: false,
    showSize: true
  };

  /*
   * 列描述
   */
  columns: STColumn[] = [];

  constructor(protected apiService: CrudRestServiceBase<TEntity>) {
  }

  ngOnInit(): void {
    this.loadLazy()
  }

  ngOnDestroy(): void {
  }

  loadData(): Observable<PagedResult<TEntity>> {
    return this.apiService.page(this.pageIndex, this.pageSize, this.query)
  }

  /*
   * 加载当前页码数据
   */
  loadLazy(): void {
    this.loading = true;
    setTimeout(() => {
      this.loadData()
        .pipe(tap(() => {
          this.loading = false;
          this.refreshStatus();
        }))
        .subscribe(data => {
          if (data) {
            this.data = data.items;
            this.total = data.total;
          } else {
            this.data = [];
            this.total = 0;
          }
        })
    }, 10);
  }

  /*
   * 页码值为1并加载数据
   */
  search(query: any = null) {
    this.pageIndex = 1;
    this.query = deepCopy(query);
    this.loadLazy();
  }

  /*
   * 清空行选择状态
   */
  refreshStatus(): void {
    this.selectedItems = [];
  }

  onStChange(e: STChange) {
    if (e.type == 'pi') {
      this.pageIndex = e.pi;
      this.pageSize = e.ps;
      this.loadLazy();
    } else if (e.type == 'ps') {
      this.pageIndex = 1;
      this.pageSize = e.ps;
      this.loadLazy();
    } else if (e.type == 'checkbox') {
      this.selectedItems = e.checkbox;
    }
  }
}

@Directive()
export abstract class CrudTableComponentBase<TEntity = any> extends TableComponentBase<TEntity> implements OnInit, OnDestroy {

  /*
   * 删除确认提示描述
   */
  deleteConfirmMessage = "确定要删除该记录吗？";

  /*
   * 编辑视图组件, Type: Component
   */
  editComponent: any;

  constructor(protected apiService: CrudRestServiceBase<TEntity>,
              protected modal: ModalHelper,
              protected notificationService: NotificationService) {
    super(apiService);
  }

  ngOnInit(): void {
    super.ngOnInit();
  }

  add() {
    this.modal
      .createStatic(this.editComponent)
      .subscribe(() => {
        this.onSaveSuccess();
      });
  }

  delete() {
    if (this.selectedItems == null || this.selectedItems.length == 0) {
      this.notificationService.warningMessage("请选择");
      return;
    }

    this.notificationService.confirmModal(this.deleteConfirmMessage, () => {
      this.apiService.delete(this.selectedItems.map(t => t.id))
        .subscribe(() => {
          this.onDeleteSuccess();
        });
    });
  }

  onSaveSuccess() {
    this.notificationService.successMessage("保存成功");
    this.loadLazy();
  }

  onDeleteSuccess() {
    this.notificationService.successMessage("删除成功");
    this.loadLazy();
  }


}

