import { ChangeDetectionStrategy, Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { VoteDialogComponent } from '../dialogs/vote-dialog/vote-dialog.component';
import { AddVoterDialogComponent } from '../dialogs/add-voter-dialog/add-voter-dialog.component';
import { AddCandidateDialogComponent } from '../dialogs/add-candidate-dialog/add-candidate-dialog.component';
import { Subscriptions } from 'src/app/shared/utils/subscriptions';
import { VotingStateService } from '../voting-state.service';

@Component({
  selector: 'tbr-vote-main-view',
  templateUrl: './vote-main-view.component.html',
  styleUrls: ['./vote-main-view.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class VoteMainViewComponent implements OnInit, OnDestroy {
  displayedColumnsVoters: string[] = ['name', 'hasVoted'];
  displayedColumnsCandidates: string[] = ['name', 'votes'];

  voters$ = this._votingStateService.voters$;
  candidates$ = this._votingStateService.candidates$;

  private _subscripions = new Subscriptions();

  constructor(private _dialog: MatDialog, private _votingStateService: VotingStateService) { }

  ngOnInit() {
    this._votingStateService.fetchVoters();
    this._votingStateService.fetchCandidates();
  }

  onAddVoter() {
    this._dialog.open(AddVoterDialogComponent);
  }

  onAddCandidate() {
    this._dialog.open(AddCandidateDialogComponent);
  }

  onVote() {
    this._dialog.open(VoteDialogComponent);
  }

  ngOnDestroy(): void {
    this._subscripions.unsubscribe();
  }
}
