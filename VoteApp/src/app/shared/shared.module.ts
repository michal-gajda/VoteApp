import { NgModule } from '@angular/core';
import { LoaderComponent } from './components/loader/loader.component';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { CommonModule } from '@angular/common';
import { ErrorDialogComponent } from './dialogs/error-dialog/error-dialog.component';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { TestComponent } from './components/test/test.component';

@NgModule({
    declarations: [
        LoaderComponent,
        ErrorDialogComponent,
        TestComponent
    ],
    imports: [
        CommonModule,
        MatProgressSpinnerModule,
        MatDialogModule,
        MatButtonModule,
        MatIconModule
    ],
    exports: [
        LoaderComponent,
        ErrorDialogComponent
    ],
    providers: []
})
export class SharedModule { }
