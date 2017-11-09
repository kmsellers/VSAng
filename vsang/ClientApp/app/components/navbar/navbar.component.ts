import { Component, Input } from '@angular/core';

@Component({
    selector: 'nav-bar',
    templateUrl: './navbar.component.html',
    styleUrls: ['./navbar.component.css']
})
export class NavBarComponent {
    //@Input() navbarItems: string
    @Input() navbarItems: string; 
}
