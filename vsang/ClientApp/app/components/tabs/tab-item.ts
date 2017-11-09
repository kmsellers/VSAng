import { Type } from '@angular/core';
import { ITabComponent } from './tab-component.interface';

export class TabItem {
    constructor(public title: string, public iconName: any, public ref: any,
                public component: Type<any>, public data: any) { }
}

