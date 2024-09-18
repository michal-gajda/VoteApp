import { Injectable } from '@angular/core';
import { VotingApiService } from '@vote-app/api';
import { AddCandidateCommand, AddVoterCommand, ListCandidatesItem, ListVotersItem, VoteCommand } from '@vote-app/models';
import { BehaviorSubject } from 'rxjs';
import { tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
/*
 * [DESC]
 * VotingState is the example of nano implementation of FLUX pattern.
 * Items of BehaviorSubject play role of stores and the methods are actons.
 */
export class VotingStateService {
  private votersSubject = new BehaviorSubject<ListVotersItem[]>([]);
  voters$ = this.votersSubject.asObservable();

  private candidatesSubject = new BehaviorSubject<ListCandidatesItem[]>([]);
  candidates$ = this.candidatesSubject.asObservable();

  constructor(private _votingApiService: VotingApiService) { }

  fetchVoters(): void {
    this._votingApiService.listVoters()
      .subscribe(voters => this.votersSubject.next(voters));
  }

  addVoter(payload: AddVoterCommand): void {
    this._votingApiService.addVoter(payload)
      .pipe(
        tap(response => {
          const voters = [...this.votersSubject.value, { id: response.id, name: payload.name, hasVoted: false }];
          this.votersSubject.next(voters);
        })
      )
      .subscribe();
  }

  fetchCandidates(): void {
    this._votingApiService.listCandidates()
      .subscribe(candidates => this.candidatesSubject.next(candidates));
  }

  addCandidate(payload: AddCandidateCommand): void {
    this._votingApiService.addCandidate(payload)
      .pipe(
        tap(response => {
          const candidates = [...this.candidatesSubject.value, { id: response.id, name: payload.name, votes: 0 }];
          this.candidatesSubject.next(candidates);

          const voters = [...this.votersSubject.value, { id: response.id, name: payload.name, hasVoted: false }];
          this.votersSubject.next(voters);
        })
      )
      .subscribe();
  }

  vote(payload: VoteCommand): void {
    this._votingApiService.vote(payload)
      .subscribe(() => {
        this.fetchVoters();
        this.fetchCandidates();
      });
  }

}
