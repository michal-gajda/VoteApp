import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AddCandidateCommand, AddCandidateResponse, AddVoterCommand, AddVoterResponse, ListVotersItem, VoteCommand } from '@vote-app/models';
import { Observable } from 'rxjs';
import { ListCandidatesItem } from '../models/ListCandidatesItem';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class VotingApiService {
  private _baseUrl = `${environment.apiUrl}/voting`;

  constructor(private _http: HttpClient) { }

  addVoter(dto: AddVoterCommand): Observable<AddVoterResponse> {
    const url = `${this._baseUrl}/add-voter`;
    return this._http.post<AddVoterResponse>(url, dto);
  }

  addCandidate(dto: AddCandidateCommand): Observable<AddCandidateResponse> {
    const url = `${this._baseUrl}/add-candidate`;
    return this._http.post<AddCandidateResponse>(url, dto);
  }

  vote(dto: VoteCommand): Observable<void> {
    const url = `${this._baseUrl}/vote`;
    return this._http.post<void>(url, dto);
  }

  listVoters(): Observable<ListVotersItem[]> {
    const url = `${this._baseUrl}/voters`;
    return this._http.get<ListVotersItem[]>(url, {});
  }

  listCandidates(): Observable<ListCandidatesItem[]> {
    const url = `${this._baseUrl}/candidates`;
    return this._http.get<ListCandidatesItem[]>(url, {});
  }

}
