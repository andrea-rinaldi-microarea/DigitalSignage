import { Component, OnInit } from '@angular/core';
import { MenuItem } from '../../models/menu-item';
import { MenuService } from '../../services/menu.service';
import { ConnectionService, ConnectionStatus } from '../../services/connection.service';
import { WeeklyMenu } from '../../models/weekly-menu';

@Component({
  selector: 'app-weekly-menu',
  templateUrl: './weekly-menu.component.html',
  styleUrls: ['./weekly-menu.component.css']
})
export class WeeklyMenuComponent implements OnInit {

  week: WeeklyMenu;
  
  constructor(
    private menu: MenuService,
    private connection: ConnectionService
  ) { }

  ngOnInit() {
    this.connection.status$.subscribe(status => {
      console.log(status);
      if (status == ConnectionStatus.connected) {
        this.menu.weekMenu().subscribe( (week: WeeklyMenu) => {
          this.week = week;
          console.log(week);
        });
      }
    }, error => {
      console.log('errors')
    })

  }

  imageURL(namespace: string) {
    return this.menu.imageURL(namespace);
  }

}
