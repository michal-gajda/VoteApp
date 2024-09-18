import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { VotingStateService } from '../../voting-state.service';

@Component({
  selector: 'tbr-add-candidate-dialog',
  templateUrl: './add-candidate-dialog.component.html',
  styleUrls: ['./add-candidate-dialog.component.scss']
})
export class AddCandidateDialogComponent {
  candidateForm: FormGroup;

  constructor(
    public _dialogRef: MatDialogRef<AddCandidateDialogComponent>,
    private _fb: FormBuilder,
    private _votingStateService: VotingStateService
  ) {
    this.candidateForm = this._fb.group({
      name: ['', Validators.required]
    });
  }

  onSubmit() {
    if (this.candidateForm.valid) {
      this._votingStateService.addCandidate({ name: this.candidateForm.value.name });
      this._dialogRef.close();
    }
  }
}
