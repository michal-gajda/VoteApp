import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ListVotersItem } from '@vote-app/models';
import { ListCandidatesItem } from 'src/app/models/ListCandidatesItem';
import { VotingStateService } from '../../voting-state.service';
import { map } from 'rxjs/operators';

@Component({
  selector: 'tbr-vote-dialog',
  templateUrl: './vote-dialog.component.html',
  styleUrls: ['./vote-dialog.component.scss']
})
export class VoteDialogComponent {
  voteForm: FormGroup;
  voters: ListVotersItem[] = [];
  candidates: ListCandidatesItem[] = [];

  voters$ = this._votingStateService.voters$.pipe(map(voters => voters.filter(voter => voter.hasVoted === false)));
  candidates$ = this._votingStateService.candidates$;

  constructor(
    public _dialogRef: MatDialogRef<VoteDialogComponent>,
    private _fb: FormBuilder,
    private _votingStateService: VotingStateService
  ) {
    this.voteForm = this._fb.group({
      selectedVoter: [null, Validators.required],
      selectedCandidate: [null, Validators.required]
    });

    this._votingStateService.fetchVoters();
    this._votingStateService.fetchCandidates();
  }

  onSubmit() {
    if (this.voteForm.valid) {
      this._votingStateService.vote({ voterId: this.voteForm.value.selectedVoter.id, candidateId: this.voteForm.value.selectedCandidate.id });
      this._dialogRef.close();
    }
  }
}
