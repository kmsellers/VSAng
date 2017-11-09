//TODO:  determine interface name should be changed.  
//       This should be the inner template component of the Tab. 
//       The tab being just the Title and what it refers to {OVerview, History, ... } 
//       and possibly the related links.  
//       inner template component should not show until Tab is open. 
//       Also, tab should retain state based on some variant, like the contact id.  
//       Is that data? should we check for change in data?  to refresh template component?   Hmmmmm..... 
//       but really, the inner template component should be notified and refresh itself and change whatever state. 
//       and the controller of the data is in layers above.  
//       Which makes me wonder why we need this data and interface...       See how this plays out  -wish it was easier to test. 

export interface ITabComponent {
    data: any;
}