"use server"
import { Person, PrismaClient } from '@prisma/client'

const prisma = new PrismaClient()

export async function getPersons(): Promise<Person[]> {
  return await prisma.person.findMany()
}
