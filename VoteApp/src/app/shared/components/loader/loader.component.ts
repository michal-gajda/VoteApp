import { Component, Input } from '@angular/core';

@Component({
  selector: 'tbr-loader',
  templateUrl: './loader.component.html',
  styleUrls: ['./loader.component.scss']
})
export class LoaderComponent {
  @Input() loading = false;
}
