import { BrowserModule } from '@angular/platform-browser';
import { NgModule, CUSTOM_ELEMENTS_SCHEMA, Injector } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from './shared/shared.module';
import { ErrorInterceptor } from './infrastructure/interceptors/error-interceptor';
import { MainLayoutComponent } from './layout/main-layout/main-layout.component';
import { MatTableModule } from '@angular/material/table';
import { VoteMainViewComponent } from './voting/vote-main-view/vote-main-view.component';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialogModule } from '@angular/material/dialog';
import { MatSelectModule } from '@angular/material/select';
import { VoteDialogComponent } from './voting/dialogs/vote-dialog/vote-dialog.component';
import { VotingApiService } from './api';
import { AddVoterDialogComponent } from './voting/dialogs/add-voter-dialog/add-voter-dialog.component';
import { AddCandidateDialogComponent } from './voting/dialogs/add-candidate-dialog/add-candidate-dialog.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { CommonModule } from '@angular/common';

@NgModule({
    declarations: [
        AppComponent,
        MainLayoutComponent,
        VoteMainViewComponent,
        VoteDialogComponent,
        AddVoterDialogComponent,
        AddCandidateDialogComponent
    ],
    imports: [
        CommonModule,
        BrowserModule,
        AppRoutingModule,
        BrowserAnimationsModule,
        FormsModule,
        ReactiveFormsModule,
        HttpClientModule,
        SharedModule,
        MatTableModule,
        MatButtonModule,
        MatIconModule,
        MatDialogModule,
        MatSelectModule,
        MatFormFieldModule,
        MatInputModule
    ],
    providers: [
        { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
        VotingApiService
    ],
    bootstrap: [AppComponent],
    schemas: [CUSTOM_ELEMENTS_SCHEMA]
})

export class AppModule {
    static injector: Injector;

    constructor(injector: Injector) {
        AppModule.injector = injector;
    }
}