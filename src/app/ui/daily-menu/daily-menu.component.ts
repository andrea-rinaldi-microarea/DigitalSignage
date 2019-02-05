import { MenuService } from './../../services/menu.service';
import { Component, OnInit } from '@angular/core';
import { MenuItem } from './../../models/menu-item';

@Component({
  selector: 'app-daily-menu',
  templateUrl: './daily-menu.component.html',
  styleUrls: ['./daily-menu.component.css']
})
export class DailyMenuComponent implements OnInit {

  items: MenuItem[];

  constructor(private menu: MenuService) { }

  ngOnInit() {
    this.menu.todayItems().subscribe( (items: MenuItem[]) => {
      this.items = items;
      console.log(items);
    })
  }

  imageURL(namespace: string) {
    return this.menu.imageURL(namespace);
  }


}
