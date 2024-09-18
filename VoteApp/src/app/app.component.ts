import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';

export const KEEPALIVE_MINUTES = 5.5;

@Component({
  selector: 'tbr-app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

  ngOnInit(): void {
    console.log(`api: ${environment.apiUrl}`);
    console.log(`location: ${location.href}`);
    console.log(`location protocol: ${location.protocol}`);
  }
}
