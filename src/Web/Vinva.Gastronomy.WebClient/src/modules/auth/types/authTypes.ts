export interface User {
    id: string;
    login: string | null;
    email: string | null;
    state: 'Active' | 'Blocked' | 'Unactive';
    roles: Array<'Client' | 'Manager' | 'Admin'> | null;
}