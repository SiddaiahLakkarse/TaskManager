export type TaskStatus = 'ToDo' | 'InProgress' | 'Completed'; export type TaskPriority = 'Low' | 'Medium' | 'High';
export interface User { userId: string; name: string; email: string; } export interface AuthResponse extends User { token: string; }
export interface Task { id: string; title: string; description?: string; status: TaskStatus; priority: TaskPriority; dueDate?: string; createdAt: string; updatedAt: string; userId: string; }
export interface TaskRequest { title: string; description?: string; status: TaskStatus; priority: TaskPriority; dueDate?: string; }
