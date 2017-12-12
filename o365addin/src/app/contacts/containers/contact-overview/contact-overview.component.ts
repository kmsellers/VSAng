import { Component, OnInit, Input, ChangeDetectionStrategy } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { ITabComponent } from "../../../components/tabs/tab-component.interface";
import { UserService } from "../../../core/user.service";

@Component({
  selector: 'app-contact-overview',
  templateUrl: './contact-overview.component.html',
  styleUrls: ['./contact-overview.component.css']
})
export class ContactOverviewComponent implements OnInit, ITabComponent {
  contactId: any;
  id: any;
  data: any;
    ovCounter: number; 
   // @Input() id: string;  ??????

    //TODO:  need to understand how a tab component can also work with a route
    //       Want to be able to load tab based on activated route. 
    //      This means that contact/:id/overview will load the contact  and go to the overview tab page
    //      without losing history information as long as the the same id is used. 
    //constructor(private route: ActivatedRoute) {
    //    this.route.params.subscribe(params => {
    //        console.log(params);
    //        this.data = params;
    //    });
    //}

    constructor(private route: ActivatedRoute, private userService: UserService)
    {

    }
    ngOnInit() {
      this.contactId = this.route.parent.snapshot.params.contactId; 
      this.route.params.subscribe(params => this.id = params.id); 
      console.log("overview child id: " + this.id);

        //Load Overview information from data.id; 
        this.ovCounter = this.userService.incOverview(); 
  }

}
