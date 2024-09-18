import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpHandler, HttpRequest, HttpEvent } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ErrorResultDto } from '@vote-app/models';
import { MatDialog } from '@angular/material/dialog';
import { ErrorDialogComponent } from 'src/app/shared/dialogs/error-dialog/error-dialog.component';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

    constructor(private _dialog: MatDialog) { }

    intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
        return next.handle(request).pipe(catchError(err => {

            if (err.error.errorLogId) {
                const errorResult = err.error as ErrorResultDto;
                this.showError(errorResult.messages);
            }
            else {
                this.showError(['Something went wrong. Please contact administrator.']);
            }

            return throwError(err);
        }))
    }

    showError(errorMessages: string[]) {
        this._dialog.open(ErrorDialogComponent, {
            width: '300px',
            data: { errorMessages }
        });
    }
}
