import http from 'k6/http'
import { check, sleep } from 'k6'
import { Counter } from 'k6/metrics'

const success_requests = new Counter('success_requests');
 
export const options = {
    VUs: 10000,
    duration: '1s',
}

export default function () {
    let result = http.get("http://localhost:5063/a1727558387");

    var is_success = check(result, { 'Request is success': (r) => r.status === 200 });
    if (is_success) success_requests.add(1);

    sleep(0.1);
}