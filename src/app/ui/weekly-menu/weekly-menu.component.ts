import { Component, OnInit } from '@angular/core';
import { MenuItem } from '../../models/menu-item';
import { MenuService } from '../../services/menu.service';
import { ConnectionService, ConnectionStatus } from '../../services/connection.service';

@Component({
  selector: 'app-weekly-menu',
  templateUrl: './weekly-menu.component.html',
  styleUrls: ['./weekly-menu.component.css']
})
export class WeeklyMenuComponent implements OnInit {

  items: MenuItem[];
  
  constructor(
    private menu: MenuService,
    private connection: ConnectionService
  ) { }

  ngOnInit() {
    this.connection.status$.subscribe(status => {
      console.log(status);
      if (status == ConnectionStatus.connected) {
        this.menu.weekItems().subscribe( (items: MenuItem[]) => {
          this.items = items;
          console.log(items);
        });
      }
    }, error => {
      console.log('errors')
    })

  }

}
