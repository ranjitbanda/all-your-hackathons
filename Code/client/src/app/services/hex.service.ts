import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable, NgZone } from '@angular/core';
import { FormattedAddress } from '../models/FormattedAddress';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HexService {

  private hexService = 'http://localhost:52483/api/Default';
  speechRecognition: any;
  blnStop: boolean;
  private httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private http: HttpClient, private zone: NgZone) { }

  getAddress(rawAddress: string) {
    const content: string = JSON.stringify(rawAddress);
    return this.http.post<FormattedAddress>(this.hexService, content, this.httpOptions);
  }

  record(): Observable<string> {
    this.blnStop = false;
    return Observable.create(observer => {
      const { webkitSpeechRecognition }: any = <any>window;
      this.speechRecognition = new webkitSpeechRecognition();
      this.speechRecognition.continuous = true;
      //this.speechRecognition.interimResults = true;
      this.speechRecognition.lang = 'en-us';
      this.speechRecognition.maxAlternatives = 1;

      this.speechRecognition.onresult = speech => {
        let term: string = "";
        if (speech.results) {
          var result = speech.results[speech.resultIndex];
          var transcript = result[0].transcript;
          if (result.isFinal) {
            if (result[0].confidence < 0.3) {
              console.log("Unrecognized result - Please try again");
            }
            else {
              term = transcript;
              console.log("Did you said? -> " + term + " , If not then say something else...");
            }
          }
        }
        this.zone.run(() => {
          observer.next(term);
        });
      };

      this.speechRecognition.onerror = error => {
        observer.error(error);
      };

      this.speechRecognition.onend = () => {
        observer.complete();
      };

      if (!this.blnStop) {
        this.speechRecognition.start();
        console.log("Say something - We are listening !!!");
      }
    });
  }

  DestroySpeechObject() {
    this.blnStop = true;
    if (this.speechRecognition) {
      console.log("entered into DestroySpeechObject")
      this.speechRecognition.stop();
    }
  }

}
