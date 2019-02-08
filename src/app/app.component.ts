import { ConnectionService, ConnectionStatus } from './services/connection.service';
import { Component, OnInit, HostListener, OnDestroy } from '@angular/core';
import { MenuItem } from './models/menu-item';
import { Router, NavigationEnd } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit, OnDestroy {
  
  connectionStatus: ConnectionStatus = ConnectionStatus.notConnected;
  errorMessage: string = null;
  currentPage: string = '/weekly';

  constructor(
    private connection: ConnectionService,
    private router: Router
  ) { 
    router.events.subscribe((url:any) => {
      if (url instanceof NavigationEnd) {
        console.log(url.url);
        this.currentPage = url.url;
      }
    });
  }

  ngOnInit() {
    this.connection.connect().subscribe( status => {
      this.connectionStatus = status;
    }, error => {
      console.log(error);
      this.errorMessage = "Connection failed: " + error;
    });
  }
  
  onChangePage() {
    this.router.navigateByUrl(this.currentPage === '/weekly' || this.currentPage === '/' ? '/daily' : '/weekly');
  }

  @HostListener('window:beforeunload', ['$event'])
    unloadNotification($event: any) {
        this.connection.disconnect();
    }

    ngOnDestroy() {
      this.connection.disconnect();
    }
}
