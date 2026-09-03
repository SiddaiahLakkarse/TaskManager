import { Routes } from '@angular/router';
import { authGuard } from './auth.guard';
import { DashboardComponent } from './dashboard.component';
import { LoginComponent } from './login.component';
import { RegisterComponent } from './register.component';
import { TaskListComponent } from './task-list.component';
import { TaskFormComponent } from './task-form.component';
import { TaskDetailsComponent } from './task-details.component';
import { NotFoundComponent } from './not-found.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent }, { path: 'register', component: RegisterComponent },
  { path: '', component: DashboardComponent, canActivate: [authGuard], children: [
    { path: '', component: TaskListComponent }, { path: 'tasks/new', component: TaskFormComponent },
    { path: 'tasks/:id/edit', component: TaskFormComponent }, { path: 'tasks/:id', component: TaskDetailsComponent }
  ] }, { path: '**', component: NotFoundComponent }
];
