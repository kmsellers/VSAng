import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-table-list',
  templateUrl: './table-list.component.html',
  styleUrls: ['./table-list.component.css']
})
export class TableListComponent implements OnInit {
  @Input() headers: any; 
  @Input() items: any; // what we really need are common pipes and language tools that get enformced.  
  constructor() { }

  ngOnInit() {
  }

}
