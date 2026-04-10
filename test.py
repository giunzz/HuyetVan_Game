class Student:
    def __init__(self, id, score):
        self.id = id
        self.score = score

    def __lt__(self, other):
        if self.score != other.score:
            return self.score > other.score
        return self.id < other.id


n, k = map(int, input().split())

hs = []
for i in range(n):
    id, score = input().split()
    hs.append(Student(id, float(score)))

hs.sort()

for i in range(k):
    print(hs[i].id)