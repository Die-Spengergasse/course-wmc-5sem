// TIPP: npx prisma generate erzeugt nach einer Schemaänderung den Client neu
// Führe danach in VS Code mit CTRL+SHIFT+P den Befehl "Typescript: Restart TS Server" aus.
import { Prisma, PrismaClient } from '@prisma/client'
import { Faker, de, en } from '@faker-js/faker'

const faker = new Faker({ locale: [de, en] })
faker.seed(1406)
const prisma = new PrismaClient()
async function main() {
  const personData: Prisma.PersonCreateInput[] = Array
    .from({ length: 5 })
    .map(() => ({
      guid: faker.string.uuid(),
      firstname: faker.person.firstName(),
      lastname: faker.person.lastName(),
      birthDate: Math.random() > 0.5 ? faker.date.birthdate() : undefined
    }));

  await prisma.person.createMany({
    data: personData,
  });
  console.log("Database seeded.")
}

main()
  .then(async () => {
    await prisma.$disconnect()
  })
  .catch(async (e) => {
    console.error(e)
    await prisma.$disconnect()
    process.exit(1)
  })
