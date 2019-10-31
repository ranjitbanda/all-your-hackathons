import { Component, OnInit } from '@angular/core';
import { HexService } from './services/hex.service';
import { FormattedAddress } from './models/FormattedAddress';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  title = 'hex';
  rawAddress = '';
  formattedAddress$: FormattedAddress;
  speechData: string;
  showSearchButton: boolean = true;
  constructor(private hexService: HexService) { }

  ngOnInit(): void {
  }

  submit() {
    this.hexService.getAddress(this.rawAddress)
      .subscribe(data => this.formattedAddress$ = data);
  }

  deActivateSpeechSearch(): void {
    this.hexService.DestroySpeechObject();
    this.showSearchButton = true;
  }

  activateSpeechSearch(): void {
    this.showSearchButton = false;

    this.hexService.record()
      .subscribe(
        //listener
        (value) => {
          this.speechData = value;
          this.rawAddress = value;
          console.log(value);
        },
        //errror
        (err) => {
          console.log(err);
          if (err.error == "no-speech") {
            console.log("--restatring service--");
            this.activateSpeechSearch();
          }
        },
        //completion
        () => {
          this.showSearchButton = true;
          console.log("--complete--");
          this.activateSpeechSearch();
        });
  }


}
