import { Employee } from '@/modules/app/api/settings/entities/employee';
import { UpdateEmployee } from '@/modules/app/api/settings/data-transfer-objects/update-employee';
import { CreateEmployee } from '@/modules/app/api/settings/data-transfer-objects/create-employee';
import { http } from '@/core';

export class EmployeeService {
    public async getEmployees(): Promise<Employee[]> {
        var res = await http.get<any[]>('/settings/employee');
        return res.data.map(r => {
            var e = new Employee(r.name, r.position);
            e.id = r.id;

            return e;
        });
    }

    public async createEmployee(createEmployee: CreateEmployee): Promise<Employee> {
        var res = await http.post('/settings/employee', createEmployee);
        var e = new Employee(res.data.name, res.data.position);
        e.id = res.data.id;

        return e;
    }

    public async updateEmployee(updateEmployee: UpdateEmployee): Promise<Employee> {
        var res = await http.put(`/settings/employee/${updateEmployee.id}`, updateEmployee);
        var e = new Employee(res.data.name, res.data.position);
        e.id = res.data.id;

        return e;
    }

    public async deleteEmployee(employee: Employee): Promise<void> {
        await http.delete(`/settings/employee/${employee.id}`);
    }
}
