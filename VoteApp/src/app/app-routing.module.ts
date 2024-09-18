import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { VoteMainViewComponent } from './voting/vote-main-view/vote-main-view.component';

const routes: Routes = [
  { path: 'vote', component: VoteMainViewComponent },
  { path: '', redirectTo: 'vote', pathMatch: 'full' },
  { path: 'home', redirectTo: 'vote', pathMatch: 'full' },
  //{ path: '**', component: PageNotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
