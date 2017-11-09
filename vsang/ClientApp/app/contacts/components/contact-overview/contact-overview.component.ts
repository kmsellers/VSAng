import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ITabComponent } from "../../../components/tabs/tab-component.interface";

@Component({
  selector: 'app-contact-overview',
  templateUrl: './contact-overview.component.html',
  styleUrls: ['./contact-overview.component.css']
})
export class ContactOverviewComponent implements OnInit, ITabComponent {
    data: any;
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

    ngOnInit() {

        //Load Overview information from data.id; 
  }

}
