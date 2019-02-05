import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';

import { AppComponent } from './app.component';
import { ConnectionStatusComponent } from './ui/connection-status/connection-status.component';
import { ConnectionService } from './services/connection.service';
import { MenuService } from './services/menu.service';
import { DailyMenuComponent } from './ui/daily-menu/daily-menu.component';
import { WeeklyMenuComponent } from './ui/weekly-menu/weekly-menu.component';

const ROUTES = [
  {
    path: '',
    redirectTo: 'daily',
    pathMatch: 'full'
  },
  {
    path: 'daily',
    component: DailyMenuComponent
  },
  {
    path: 'weekly',
    component: WeeklyMenuComponent
  }
];

@NgModule({
  declarations: [
    AppComponent,
    ConnectionStatusComponent,
    DailyMenuComponent,
    WeeklyMenuComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    RouterModule.forRoot(ROUTES),
    NgbModule
  ],
  providers: [ConnectionService, MenuService],
  bootstrap: [AppComponent]
})
export class AppModule { }
