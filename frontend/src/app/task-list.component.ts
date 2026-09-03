import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { TaskService } from './task.service';
import { Task } from './models';
@Component({ standalone: true, imports: [CommonModule, RouterLink, FormsModule, MatButtonModule, MatFormFieldModule, MatInputModule, MatSelectModule], template: `<header><h1>My tasks</h1><a mat-flat-button color="primary" routerLink="/tasks/new">New task</a></header><div class="filters"><mat-form-field><mat-label>Search</mat-label><input matInput [(ngModel)]="search" (ngModelChange)="load()"></mat-form-field><mat-form-field><mat-label>Status</mat-label><mat-select [(ngModel)]="status" (selectionChange)="load()"><mat-option value="">All</mat-option><mat-option value="ToDo">To do</mat-option><mat-option value="InProgress">In progress</mat-option><mat-option value="Completed">Completed</mat-option></mat-select></mat-form-field></div><table><tr><th>Title</th><th>Status</th><th>Priority</th></tr><tr *ngFor="let task of tasks"><td><a [routerLink]="['/tasks', task.id]">{{task.title}}</a></td><td>{{task.status}}</td><td>{{task.priority}}</td></tr></table>` })
export class TaskListComponent { private service = inject(TaskService); tasks: Task[] = []; search = ''; status = ''; ngOnInit() { this.load(); } load() { this.service.list({ search: this.search, status: this.status }).subscribe(x => this.tasks = x); } }
