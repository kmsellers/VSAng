import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { ITabComponent } from "../../../components/tabs/tab-component.interface";
import { ActivatedRoute } from '@angular/router';
import { ContactService } from '../../services/contact.service';
import { SalesHistory } from '../../models/sales-history';
import { Subscription } from 'rxjs/Subscription';
//import { TableListComponent } from "../../../components/table-list/table-list.component";
@Component({
  selector: 'app-contact-history',
  templateUrl: './contact-history.component.html',
  styleUrls: ['./contact-history.component.css']
})
export class ContactHistoryComponent implements OnInit, ITabComponent {
  salesHistorySub: Subscription;
  @Input() data: any;   
  contactId: any;  
  salesHistory: SalesHistory;
  



  constructor(private route: ActivatedRoute, private contactService: ContactService) { }

  ngOnInit(): void {
    this.route.parent.params.subscribe(params => {
        this.contactId = params.contactId; 
        this.salesHistorySub = this.contactService.getContactSalesHistory(this.contactId)
          .subscribe(s => this.salesHistory = s, () => {console.log("sales history completed.")}); 
    });
    
  }
  ngOnDestroy() : void {

    this.salesHistorySub.unsubscribe(); 
  }

}
