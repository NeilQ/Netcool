import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { PagedResult } from '@models';
import { buildQueryString } from "@core/common";

@Injectable()
export abstract class CrudRestServiceBase<T> {

  constructor(protected apiPrefix: string,
              protected http: HttpClient) {
  }

  add(entity: T): Observable<any> {
    return this.http.post(this.apiPrefix, entity);
  }

  update(id: number, entity: T): Observable<any> {
    return this.http.put(this.apiPrefix + '/' + id, entity);
  }

  delete(ids: number[]): Observable<any> {
    if (ids.length === 1) {
      return this.http.delete(this.apiPrefix + '/' + ids[0]);
    } else {
      return this.http.post(this.apiPrefix + '/delete', ids);
    }
  }

  get(id: number): Observable<T> {
    return this.http.get<T>(this.apiPrefix + '/' + id)
  }

  list(query: any = null): Observable<T[]> {
    if (query == null) {
      query = {};
    }
    return this.http.get<T[]>(this.apiPrefix + "/items" + buildQueryString(query));
  }

  page(page: number, size: number, query: any = null): Observable<PagedResult<T>> {
    if (query == null) {
      query = {};
    }
    query.page = page;
    query.size = size;
    return this.http.get<PagedResult<T>>(this.apiPrefix + buildQueryString(query));
  }

}

