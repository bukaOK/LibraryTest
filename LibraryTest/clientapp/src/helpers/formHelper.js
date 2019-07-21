export default {
    parseErrors(errors) {
        const parsed = []
        let i = 0
        for (let key in errors) {
            for (let err of errors[key]) {
                parsed.push({
                    id: i,
                    message: err
                })
                i++
            }
        }
        return parsed
    },
    validations: {
        required(v, vname){
            return !!v || `Заполните поле ${vname}`
        },
        email(v){            
            return /^[a-zA-Z0-9.!#$%&’*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/.test(v) || 'Неверный e-mail'
        },
        number(v){
            return !isNaN(+v) || 'Поле должно быть числом'
        }
    }
}