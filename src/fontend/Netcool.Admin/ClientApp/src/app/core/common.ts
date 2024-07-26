import { HttpErrorResponse } from "@angular/common/http";
import { throwError } from "rxjs";

export function buildQueryString(query: any) {
  if (query == null) {
    return '';
  }
  let first = true;
  let str = '';
  for (let key in query) {
    if (!query.hasOwnProperty(key)) {
      continue;
    }
    if (key.indexOf('_ignored') !== -1) {
      continue;
    }
    if (query[key] == null || query[key] === '') {
      continue;
    }
    if (first) {
      str += '?';
    } else {
      str += '&';
    }
    str += `${key}=${query[key]}`;
    first = false;
  }
  return str;
}

export function extractHttpError(error: HttpErrorResponse) {
  let msg = '';

  if (error.error instanceof ErrorEvent) {
    // A client-side or network error occurred. Handle it accordingly.
    console.error('An error occurred:', error.error.message);
    msg = 'An error occurred:' + error.error.message;
  } else {
    console.error(
      `Backend returned code ${error.status}, ` +
      `body was: ${error.error}`);
    if (error.error && (error.error.Message || error.error.message)) {
      msg = error.error.Message || error.error.message;
    } else {
      msg = error.error || error.statusText;
    }
  }

  // return an observable with a user-facing error message
  return throwError(msg);
}

export function extractHttpErrorMessage(error: HttpErrorResponse): string {
  let msg = '';

  if (error.error instanceof ErrorEvent) {
    // A client-side or network error occurred. Handle it accordingly.
    console.error('An error occurred:', error.error.message);
    msg = 'An error occurred:' + error.error.message;
  } else {
    console.error(
      `Backend returned code ${error.status}, ` +
      `url was: ${error.url}` +
      `body was: ${error.error}`);
    if (error.error && (error.error.Message || error.error.message)) {
      msg = error.error.Message || error.error.message;
    } else {
      msg = error.error || error.statusText;
    }
  }

  // return an observable with a user-facing error message
  return msg;
}

