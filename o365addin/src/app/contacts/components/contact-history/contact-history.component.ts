import { Component, OnInit, Input } from '@angular/core';
import { ITabComponent } from "../../../components/tabs/tab-component.interface";

@Component({
  selector: 'app-contact-history',
  templateUrl: './contact-history.component.html',
  styleUrls: ['./contact-history.component.css']
})
export class ContactHistoryComponent implements OnInit, ITabComponent {
    @Input() data: any; 

  constructor() { }

  ngOnInit() {
  }

}
