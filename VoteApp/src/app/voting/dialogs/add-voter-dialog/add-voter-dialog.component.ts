import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { VotingStateService } from '../../voting-state.service';

@Component({
  selector: 'tbr-add-voter-dialog',
  templateUrl: './add-voter-dialog.component.html',
  styleUrls: ['./add-voter-dialog.component.scss']
})
export class AddVoterDialogComponent {
  voterForm: FormGroup;

  constructor(
    public _dialogRef: MatDialogRef<AddVoterDialogComponent>,
    private _fb: FormBuilder,
    private _votingStateService: VotingStateService
  ) {
    this.voterForm = this._fb.group({
      name: ['', Validators.required]
    });
  }

  onSubmit() {
    if (this.voterForm.valid) {
      this._votingStateService.addVoter({ name: this.voterForm.value.name });
      this._dialogRef.close();
    }
  }
}
