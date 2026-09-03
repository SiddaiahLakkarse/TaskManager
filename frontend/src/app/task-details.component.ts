import { DatePipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { TaskService } from './task.service';
import { Task } from './models';
@Component({ standalone: true, imports: [DatePipe, RouterLink, MatButtonModule], template: `@if (task) {<h1>{{task.title}}</h1><p>{{task.description}}</p><p>Status: {{task.status}} · Priority: {{task.priority}}</p><p>Due: {{task.dueDate | date:'mediumDate'}}</p><a mat-button [routerLink]="['/tasks', task.id, 'edit']">Edit</a><button mat-button (click)="complete()">Mark completed</button><button mat-button color="warn" (click)="remove()">Delete</button>} @else {<p>Loading...</p>}` })
export class TaskDetailsComponent { private route = inject(ActivatedRoute); private router = inject(Router); private service = inject(TaskService); task?: Task; ngOnInit() { this.service.get(this.route.snapshot.paramMap.get('id')!).subscribe(t => this.task = t); } complete() { this.service.complete(this.task!.id).subscribe(() => this.task!.status = 'Completed'); } remove() { this.service.delete(this.task!.id).subscribe(() => this.router.navigateByUrl('/')); } }
